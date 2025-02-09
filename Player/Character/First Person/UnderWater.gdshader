shader_type canvas_item;

uniform vec3 base_color : source_color;

uniform float min_transparency : hint_range(0.0, 1.0, 0.01);
uniform float max_transparency : hint_range(0.0, 1.0, 0.01);
uniform float wave_transparency : hint_range(0.0, .1, 0.001);
uniform float wave_speed : hint_range(0.0, 1, 0.01);

void fragment() {
	float transparency_target;

    float mag_from_center = sqrt(pow((0.5 - UV.x), 2) + pow((0.5 - UV.y), 2));
    float base_transparency  = mix(1.0 - max_transparency, 1.0 - min_transparency, mag_from_center * 2.0);

    float fluctuation = sin(TIME * wave_speed);

    float fluctuation_effect = wave_transparency * mag_from_center * fluctuation;

	float transparency = smoothstep(0.0, 1.0, base_transparency + fluctuation_effect);


    // Output the final color
    COLOR = vec4(base_color, transparency);
}
