shader_type spatial;
//render_mode blend_mix;

uniform sampler2D heightMap : source_color;
uniform sampler2D normalMap : hint_normal; // could calculate from above heightmap but idc rn

uniform sampler2D gradient : source_color;

#define SAMPLEPOS UV// vec2(COLOR.x,COLOR.z)//UV
#define HEIGHT texture(heightMap,SAMPLEPOS).r

void vertex() {
	// turn color into world position please thanks no prob :p
	//COLOR.xyz = VERTEX;
	VERTEX.y = HEIGHT*10.0; // vertex displacement
}

void fragment() {

	float h = HEIGHT; // gonna do this so i reuse the sample instead of sampling twice

	vec3 waterColour = vec3(0.0);
	ALBEDO = h==0.0 ? waterColour : texture(gradient,vec2(h)).rgb;

	NORMAL_MAP = texture(normalMap,SAMPLEPOS).rgb;
}