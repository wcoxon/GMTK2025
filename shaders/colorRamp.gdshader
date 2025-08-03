shader_type spatial;
render_mode ambient_light_disabled;

uniform sampler2D ramp : source_color;

void light() {
	DIFFUSE_LIGHT = texture(ramp,vec2(dot(NORMAL,LIGHT)*0.5 + 0.5)).rgb;
}
