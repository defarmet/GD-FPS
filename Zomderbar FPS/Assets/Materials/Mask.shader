Shader "Custom/Mask"
{
	SubShader
	{
		Tags { "Queue" = "Transparent+1" }

		pass {
			Blend Zero One
		}
	}
}
