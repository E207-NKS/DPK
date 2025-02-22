package com.e207.back.repository;

import com.e207.back.entity.PlayerClassEntity;
import com.e207.back.entity.id.PlayerClassId;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Slice;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PlayerClassRepository extends JpaRepository<PlayerClassEntity, PlayerClassId> {
//    Slice<User> findByOrderByScoreDesc(Pageable pageable);
    Slice<PlayerClassEntity> findByOrderByPlayerLevelDesc(Pageable pageable);
    List<PlayerClassEntity> findByPlayerPlayerId(String playerId);

    PlayerClassEntity findTopByPlayerPlayerIdOrderByPlayerLevelDesc(String playerId);

    @Query("SELECT COUNT(p) FROM PlayerClassEntity p WHERE p.playerLevel > :playerLevel")
    int countByClassCodeAndPlayerLevelGreaterThan( @Param("playerLevel") int playerLevel);
}
