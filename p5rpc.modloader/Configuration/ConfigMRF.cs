﻿using System.ComponentModel;

namespace p5rpc.modloader.Configuration;

public class ConfigMRF
{
    [DisplayName("Intro Skip")]
    [DefaultValue(false)]
    public bool IntroSkip { get; set; } = false;

    [DisplayName("Render In Background")]
    [DefaultValue(false)]
    public bool RenderInBackground { get; set; } = false;

    [DisplayName("Force 4k Assets")]
    [DefaultValue(false)]
    public bool Force4k { get; set; } = false;
}