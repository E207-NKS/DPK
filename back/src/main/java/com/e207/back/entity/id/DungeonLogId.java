package com.e207.back.entity.id;

import jakarta.persistence.Embeddable;
import lombok.Getter;
import lombok.Setter;

import java.io.Serializable;

@Embeddable
@Getter
@Setter
public class DungeonLogId implements Serializable {

    private String dungeonCode;
    private String partyId;

    // 기본 생성자
    public DungeonLogId() {
    }

    // 매개변수 있는 생성자
    public DungeonLogId(String dungeonCode, String partyId) {
        this.dungeonCode = dungeonCode;
        this.partyId = partyId;
    }

    // getters, setters, hashCode, equals 구현
}