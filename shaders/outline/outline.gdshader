shader_type spatial;
render_mode cull_front, unshaded;


instance uniform float width = 0.0;
uniform vec4 colour = vec4(1,0,0,1);

void vertex() {
	//VERTEX += NORMAL*width;
	VERTEX *= 1.0+width;
}

void fragment() {
	ALBEDO = colour.rgb;
}

