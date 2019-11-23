package com.example.capstoneui.Models;

import java.util.List;

public class AvailableMedia {

    private List<AvailableVideo> availableVideos = null;
    private List<String> availableImages = null;

    public List<AvailableVideo> getAvailableVideos() {
        return availableVideos;
    }

    public void setAvailableVideos(List<AvailableVideo> availableVideos) {
        this.availableVideos = availableVideos;
    }

    public List<String> getAvailableImages() {
        return availableImages;
    }

    public void setAvailableImages(List<String> availableImages) {
        this.availableImages = availableImages;
    }

}
