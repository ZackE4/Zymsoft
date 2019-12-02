package com.example.capstoneui.Models;

public class UndoLogEntry {
    private UndoType type;
    private int value;
    private Boolean side;

    public Boolean getSide() {
        return side;
    }

    public void setSide(Boolean side) {
        this.side = side;
    }

    public UndoType getType() {
        return type;
    }

    public void setType(UndoType type) {
        this.type = type;
    }

    public int getValue() {
        return value;
    }

    public void setValue(int value) {
        this.value = value;
    }
}


