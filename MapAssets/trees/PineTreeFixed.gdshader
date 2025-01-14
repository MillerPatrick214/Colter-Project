shader_type spatial;
render_mode cull_disabled, diffuse_toon, specular_schlick_ggx;
// Nice Shader by @_Malido ^^


uniform sampler2D texture_albedo; // Albedo texture without type hint
uniform float albedo_darken : hint_range(0.0, 1.0, 0.01) = .1;
uniform vec3 wind_direction; // Use a negative y component to give it an extra touch (For displacement effect and noise scroll direction)
uniform float wind_strength: hint_range(0.0, 1.0, 0.01) = 0.3;
uniform sampler2D wind_noise; // Periln FBM Noise looks Best
uniform float wind_noise_size: hint_range(0.0, 1.0, 0.001) = 0.05; // high values dont work well
uniform float wind_noise_speed: hint_range(0.0, 1.0, 0.001) = 0.1;

void vertex() {
	vec3 world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;

	vec3 wind_texture = texture(wind_noise, VERTEX.xz * wind_noise_size + normalize(-wind_direction.xz) * (TIME + UV.y / 2.5) * wind_noise_speed).rgb;
	vec3 wind_vector = (vec4(wind_texture * normalize(wind_direction) * wind_strength, 0.0) * MODEL_MATRIX).xyz;
	// Apply Wind displacement linearly
	VERTEX += wind_vector * (1.0 - UV.y);
	}


void fragment() {
	float albedo_darken_inverted = 1.0 - albedo_darken;


    vec4 texture_color = texture(texture_albedo, UV);

    // Set the albedo and alpha (transparency)
    ALBEDO = texture_color.rgb * albedo_darken_inverted;
    ALPHA = texture_color.a;  // Use the alpha from the texture for transparency
	SPECULAR = 0.0;
}