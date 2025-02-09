shader_type spatial;
render_mode cull_disabled;

uniform vec3 albedo : source_color;					// so this shit adds shit to our UI
uniform vec3 albedo2 : source_color;
uniform float metallic : hint_range(0.0, 1.0) = 0;
uniform float roughness : hint_range(0.0, 1.0) = 0.02;
uniform float transparency : hint_range(0.0, 1.0) = 0;
uniform sampler2D texture_normal;
uniform sampler2D texture_normal2;

uniform vec3 fresnel_color : source_color;
uniform float fresnel_amount : hint_range(0.0, 10.0, 0.1);
uniform float fresenel_intensity : hint_range(0.0, 10.0, 0.1);

uniform vec2 wave_direction = vec2(0, -2.0);
uniform vec2 wave_direction2 = vec2(0.0,-.5);
uniform float time_scale : hint_range(0.0, 1.0, 0.01);

uniform float wave_strength : hint_range(0.0, 20.0, .01) = 0.2;

void vertex() {
	vec3 world_position = VERTEX.xyz;
	vec2 normal_map_coords = world_position.xz + (TIME / 10.0f * wave_direction) * time_scale;

	// Sample the normal map
	vec3 wave_normal = texture(texture_normal, normal_map_coords).rgb;

	// Calculate displacement and normalize it to avoid raising the entire surface
	float average_b = 0.5; // Assume the average of wave_normal.b is around 0.5 for a typical normal map
	// Calculate displacement
float displacement = (wave_normal.b - 0.5) * wave_strength;

// Subtract a constant offset to level the water (experiment to fine-tune)
displacement -= 0.5 * wave_strength; // Adjust based on the scale of wave_strength
// Apply displacement
VERTEX.y += displacement;
}


vec3 fresnel_glow(float amount, float intensity, vec3 color, vec3 normal, vec3 view)
{
	return pow((1.0 - dot(normalize(normal), normalize(view))), amount) * color * intensity;
}

void fragment() {
	//time cals for wave
	vec2 time = (TIME/10.0f * wave_direction) * time_scale; //wave1 move rate
	vec2 time2 = (TIME/10.0f * wave_direction2) * time_scale; //wave2 move rate

	vec3 normal_blend = mix(texture(texture_normal,UV + time).rgb, texture(texture_normal2, UV + time2).rgb, 0.5);

	vec3 fresnel_effect = fresnel_glow(fresnel_amount, fresenel_intensity, fresnel_color, NORMAL, VIEW);
	vec3 surface_color = mix(albedo, albedo2, fresnel_effect);

	ALBEDO = surface_color;
	METALLIC = metallic;
	ROUGHNESS = roughness;
	NORMAL_MAP = normal_blend;

	//transparency apply
	ALPHA = 1.0 - transparency;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
