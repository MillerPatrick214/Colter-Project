shader_type spatial;

uniform float fadeTime : hint_range(0.0, 1.0, .01);
uniform sampler2D trailAlbedo : filter_nearest, source_color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
