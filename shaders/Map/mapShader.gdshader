shader_type spatial;
//render_mode blend_mix;

uniform sampler2D heightMap : source_color;
uniform sampler2D normalMap : hint_normal; // could calculate from above heightmap but idc rn

uniform sampler2D gradient : source_color;

#define HEIGHT texture(heightMap,UV).r

void fragment() {

	float h = HEIGHT; // gonna do this so i reuse the sample instead of sampling twice

	vec3 waterColour = vec3(0.0);
	ALBEDO = texture(gradient,vec2(h)).rgb;
	ALPHA = step(0.01,h);

	NORMAL_MAP = texture(normalMap,UV).rgb;
}