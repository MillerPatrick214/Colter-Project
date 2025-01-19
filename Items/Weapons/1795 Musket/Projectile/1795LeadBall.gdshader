shader_type spatial;
uniform sampler2D noise1;
uniform sampler2D noise2;
uniform float noiseSpeed : hint_range(0, 1, .1) = .5;
uniform float transparency : hint_range(0.0, 1.0, 0.01) = .3;


void vertex() {


}

void fragment() {
	vec3 noiseBlend = mix(texture(noise1, noiseSpeed * TIME + UV).rgb, texture(noise2, noiseSpeed * TIME + UV).rgb, .5);
	ALBEDO = noiseBlend;
	ALPHA = 1.0f - transparency;
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
