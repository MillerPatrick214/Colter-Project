shader_type spatial;
render_mode unshaded;

uniform float pixel_size : hint_range(1.0, 500.0) = 150; // Adjust range as needed

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest;

void fragment() {
    vec2 uv = UV; // Use UV for spatial shaders

    // Apply pixelation effect
    uv = floor(uv * pixel_size) / pixel_size;

    // Sample the texture
    vec4 texColor = texture(SCREEN_TEXTURE, uv);

    ALBEDO = texColor.rgb; // Use ALBEDO instead of COLOR
}

