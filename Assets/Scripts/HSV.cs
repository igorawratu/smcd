using UnityEngine;
using System.Collections;

public class HSV 
{
    private float h_ = 0;
    public float h
    {
        get
        {
            return h_;
        }
        set
        {
            h_ = value % 360;
            h_ = Mathf.Clamp(h_, 0, 360);
        }
    }


    private float s_ = 0;
    public float s
    {
        get
        {
            return s_;
        }
        set
        {
            s_ = Mathf.Clamp(value, 0, 1);
        }
    }
    public float v_ { set; get; }
    public float v
    {
        get
        {
            return v_;
        }
        set
        {
            v_ = Mathf.Clamp(value, 0, 1);
        }
    }
    public float a_ { set; get; }
    public float a
    {
        get
        {
            return a_;
        }
        set
        {
            a_ = Mathf.Clamp(value, 0, 1);
        }
    }

	// r,g,b,a values are from 0 to 1
    // h = [0,360], s = [0,1], v = [0,1], a = [0,1]
	//		if s == 0, then h = -1 (undefined)
	public HSV()
	{
		h_ = 0;
		s_ = 0;
		v_ = 0;
		a_ = 1;
	}

	public HSV(float h, float s, float v, float a)
	{
		h_ = h;
        s_ = s;
        v_ = v;
		a_ = v;
	}

	public static HSV RGBtoHSV(Color rgb)
	{
		return RGBtoHSV( rgb.r, rgb.g, rgb.b, rgb.a);
	}

	public static HSV RGBtoHSV( float r, float g, float b, float a)
	{
		float min, max, delta;
		
		min = Mathf.Min(Mathf.Min( r, g), b );
		max = Mathf.Max(Mathf.Max( r, g), b );
		
		delta = max - min;

		HSV hsv = new HSV ();
		hsv.a_ = a;
		hsv.v_ = max;

		if (max != 0) 
		{
			hsv.s_ = delta / max;		// s
		}
		else 
		{
			// r = g = b = 0		// s = 0, v is undefined
			hsv.s_ = 0;
			hsv.h_ = -1;
			return hsv;
		}

		if (r == max) 
		{
			hsv.h_ = (g - b) / delta;		// between yellow & magenta
		} else if (g == max) 
		{
			hsv.h_ = 2 + (b - r) / delta;	// between cyan & yellow
		} else 
		{
			hsv.h_ = 4 + (r - g) / delta;	// between magenta & cyan
		}
		
		hsv.h_ *= 60;				// degrees
		if( hsv.h_ < 0 )
			hsv.h_ += 360;
		return hsv;
		
	}
	public static Color HSVtoRGB( HSV hsv )
	{
		return HSVtoRGB( hsv.h_,hsv.s_, hsv.v_,hsv.a_);
	}
	public static Color HSVtoRGB( float h, float s, float v, float a)
	{
		int i;
		float f, p, q, t;
		Color colour = new Color ();
		colour.a = a;

		if( s == 0 ) 
		{
			// achromatic (grey)
			colour.r = v;
			colour.g = v;
			colour.b = v;
			return colour;
		}
		
		h /= 60;			// sector 0 to 5
		i = Mathf.FloorToInt( h );
		f = h - i;			// factorial part of h
		p = v * ( 1 - s );
		q = v * ( 1 - s * f );
		t = v * ( 1 - s * ( 1 - f ) );

		switch( i ) 
		{
			case 0:
				colour.r = v;
				colour.g = t;
				colour.b = p;
				break;
			case 1:
				colour.r = q;
				colour.g = v;
				colour.b = p;
				break;
			case 2:
				colour.r = p;
				colour.g = v;
				colour.b = t;
				break;
			case 3:
				colour.r = p;
				colour.g = q;
				colour.b = v;
				break;
			case 4:
				colour.r = t;
				colour.g = p;
				colour.b = v;
				break;
			default:		// case 5:
				colour.r = v;
				colour.g = p;
				colour.b = q;
				break;
		}
		return colour;
	}

    public override string ToString()
	{
		return "HSV("+h_+","+s_+","+v_+")";
	}
}
