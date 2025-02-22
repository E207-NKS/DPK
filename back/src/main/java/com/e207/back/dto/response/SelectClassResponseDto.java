package com.e207.back.dto.response;

import com.e207.back.dto.ResponseDto;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

@Getter
@Setter
public class SelectClassResponseDto extends ResponseDto {

    public static ResponseEntity<? super SelectClassResponseDto> classNotFound() {
        SelectClassResponseDto responseBody = new SelectClassResponseDto();
        responseBody.setMessage("없는 직업 입니다.");
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(responseBody);
    }
}
