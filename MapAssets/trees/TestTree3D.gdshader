shader_type spatial;
render_mode cull_disabled;

uniform vec3 wind_direction; // Wind direction
uniform float wind_strength : hint_range(0.0, 1.0, 0.01) = 0.3;
uniform sampler2D wind_noise; // Perlin FBM Noise for wind effects
uniform float wind_noise_size : hint_range(0.0, 1.0, 0.001) = 0.05; // Wind noise size
uniform float wind_noise_speed : hint_range(0.0, 1.0, 0.001) = 0.1; // Wind noise speed
uniform float bend_strength : hint_range(0.0,0.1, 0.001) = 0.0; // Bend strength for the tree

uniform sampler2D texture : source_color;

void vertex() {
    // Get wind texture from the noise texture
    vec3 wind_texture = texture(wind_noise, wind_noise_size + normalize(-wind_direction.xz) * (TIME/ 2.5) * wind_noise_speed).rgb;
    vec3 wind_vector = (vec4(wind_texture * normalize(wind_direction) * wind_strength, 0.0) * MODEL_MATRIX).xyz;

    // Calculate the distance to the top in local space
    float topY = 10.0; // This value should be dynamically calculated in C# or set based on your tree size
    float distance_to_top = (17.0 - (topY - VERTEX.y));

    // Apply bending effect to Y based on distance from the top of the tree
    float bend_factor = bend_strength * pow(distance_to_top, 2);

    // Apply wind displacement effect to X and Z
    VERTEX += wind_vector * (1.0 - VERTEX.y);
	
	VERTEX.y -= (bend_factor) * sqrt(dot(VERTEX.xz, VERTEX.xz));		//magnitude of vector in both height and distance from center.



}

void fragment() {
    vec4 texColor = texture(texture, UV);
    ALBEDO = texColor.rgb;
}

