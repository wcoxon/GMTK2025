shader_type canvas_item;

uniform sampler2D tileTexture : repeat_enable;

void fragment() {
	
	COLOR = texture(tileTexture,FRAGCOORD.xy*0.05);
}
