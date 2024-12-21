shader_type spatial;

uniform vec3 albedo : source_color;					// so this shit adds shit to our UI
uniform vec3 albedo2 : source_color;
uniform float metallic : hint_range(0.0, 1.0) = 0;
uniform float roughness : hint_range(0.0, 1.0) = 0.02;
uniform float transparency : hint_range(0.0, 1.0) = 0;
uniform sampler2D texture_normal;
uniform sampler2D texture_normal2;

uniform vec2 wave_direction = vec2(0, -2.0);
uniform vec2 wave_direction2 = vec2(0.0,-.5);
uniform float time_scale : hint_range(0.0, 1.0, 0.1) = 0.01;

void vertex() {
	// Called for every vertex the material is visible on.
}

float fresnel(float amount, vec3 normal, vec3 view) {
	return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}

void fragment() {
	//time cals for wave
	vec2 time = (TIME * wave_direction) * time_scale; //wave1 move rate
	vec2 time2 = (TIME * wave_direction2) * time_scale; //wave2 move rate
	
	vec3 normal_blend = mix(texture(texture_normal,UV + time).rgb, texture(texture_normal2,UV + time2).rgb, 0.5);
	
	float fresnel = fresnel(5.0, NORMAL, VIEW);
	vec3 surface_color = mix(albedo, albedo2, fresnel);
	
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
