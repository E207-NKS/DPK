package com.e207.back.service.implement;

import com.e207.back.dto.ResponseDto;
import com.e207.back.dto.common.LoadedSkill;
import com.e207.back.dto.request.LoadSkillsRequestDto;
import com.e207.back.dto.request.SaveLearnedSkillsRequestDto;
import com.e207.back.dto.response.LoadSkillsResponseDto;
import com.e207.back.dto.response.SaveLearnedSkillsResponseDto;
import com.e207.back.entity.ClassEntity;
import com.e207.back.entity.LearnedSkillEntity;
import com.e207.back.entity.PlayerEntity;
import com.e207.back.entity.id.LearnedSkillId;
import com.e207.back.repository.ClassRepository;
import com.e207.back.repository.LearnedSkillRepository;
import com.e207.back.repository.PlayerRepository;
import com.e207.back.repository.SkillRepository;
import com.e207.back.service.SkillService;
import com.e207.back.util.CustomUserDetails;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;


@Service
@RequiredArgsConstructor
public class SkillServiceImpl implements SkillService {

    private final SkillRepository skillRepository;
    private final LearnedSkillRepository learnedSkillRepository;
    private final PlayerRepository playerRepository;
    private final ClassRepository classRepository;


    @Override
    @Transactional
    public ResponseEntity<? super SaveLearnedSkillsResponseDto> saveLearnedSkills(SaveLearnedSkillsRequestDto dto) {
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();
            String classCode = dto.getClassCode();

            Optional<PlayerEntity> player = playerRepository.findById(playerId);
            Optional<ClassEntity> classEntity = classRepository.findById(classCode);
            System.out.println(playerId);
            List<LearnedSkillEntity> skilllist = learnedSkillRepository.findByPlayerPlayerIdAndClassEntityClassCode(playerId, classCode);

            skilllist.forEach((e) -> {
                System.out.println(e.getSkill().getSkillCode());
                e.setActive(false);
                learnedSkillRepository.save(e);
            });


            dto.getSkillList().forEach((e) ->{

                Optional<LearnedSkillEntity> learnedSkill = learnedSkillRepository.findByPlayerPlayerIdAndSkillSkillCode(playerId, e.getSkillCode());
                if(learnedSkill.isPresent()){
                    learnedSkill.get().setActive(true);
                    learnedSkill.get().setSkillSlot(e.getIndex());
                    learnedSkillRepository.save(learnedSkill.get());
                }
                else{
                    LearnedSkillEntity newLearnedSkill = new LearnedSkillEntity();
                    newLearnedSkill.setSkillSlot(e.getIndex());
                    newLearnedSkill.setActive(true);
                    LearnedSkillId id = new LearnedSkillId(playerId, e.getSkillCode(), classCode);
                    newLearnedSkill.setId(id);
                    newLearnedSkill.setPlayer(player.get());
                    newLearnedSkill.setSkill(skillRepository.findById(e.getSkillCode()).get());
                    newLearnedSkill.setClassEntity(classEntity.get());
                    learnedSkillRepository.save(newLearnedSkill);

                }


            });



        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }
        return SaveLearnedSkillsResponseDto.success();
    }

    @Override
    public ResponseEntity<? super LoadSkillsResponseDto> loadSkills(LoadSkillsRequestDto dto) {
        List<LoadedSkill> skills = new ArrayList<>();
        List<LoadedSkill> commonSkills = new ArrayList<>();
        try{
            CustomUserDetails customUserDetails = CustomUserDetails.LoadUserDetails();
            String playerId = customUserDetails.getPlayerId();
            List<LearnedSkillEntity> list = learnedSkillRepository.findByPlayerPlayerIdAndActive(playerId, true);
            list.forEach((e) -> {
                LoadedSkill skill = new LoadedSkill();
                skill.setIndex(e.getSkillSlot());
                skill.setClassCode(e.getClassEntity().getClassCode());
                skill.setSkillCode(e.getSkill().getSkillCode());
                skill.setSkillName(e.getSkill().getSkillName());
                skills.add(skill);
            });

            // 공통 스킬
            list = learnedSkillRepository.findByPlayerPlayerIdAndClassEntityClassCode(playerId, "C000");
            list.forEach((e) ->{
                LoadedSkill skill = new LoadedSkill();
                skill.setIndex(-1);
                skill.setClassCode(e.getClassEntity().getClassCode());
                skill.setSkillCode(e.getSkill().getSkillCode());
                skill.setSkillName(e.getSkill().getSkillName());
                commonSkills.add(skill);

            });


        }catch (Exception exception){
            exception.printStackTrace();
            return ResponseDto.databaseError();
        }

        return LoadSkillsResponseDto.success(skills,commonSkills);
    }
}
