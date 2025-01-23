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

}

void fragment() {
}
