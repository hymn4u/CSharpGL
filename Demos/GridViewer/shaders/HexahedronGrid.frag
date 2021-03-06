﻿#version 150 core
in float pass_uv;
out vec4 out_Color;

uniform sampler1D colorCodeSampler;
uniform float renderingWireframe;
uniform float brightness = 1.0f;
uniform float opacity = 1.0f;

void main(void) {
	if (renderingWireframe > 0.0)
	{
		if (0.0 <= pass_uv && pass_uv <= 1.0)
		{
			out_Color = vec4(1, 1, 1, opacity);
		}
		else
		{
			discard;
		}
	}
	else
	{
	    if (0.0 <= pass_uv && pass_uv <= 1.0)
		{
			vec4 color = texture(colorCodeSampler, pass_uv) * brightness;
			color.a = opacity;
			out_Color = color;
		}
		else
		{
			discard;
		}
	}

}
