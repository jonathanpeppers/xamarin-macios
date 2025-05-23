#if !__MACCATALYST__
using System;
using System.Text;
using Foundation;
using ObjCRuntime;
using System.Runtime.InteropServices;

#nullable enable

namespace AppKit {
	public partial class NSColor {

		/// <param name="red">To be added.</param>
		/// <param name="green">To be added.</param>
		/// <param name="blue">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromRgb (nfloat red, nfloat green, nfloat blue)
		{
			return FromRgba (red, green, blue, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromRgb (byte red, byte green, byte blue)
		{
			return FromRgba (red / 255.0f, green / 255.0f, blue / 255.0f, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromRgb (int red, int green, int blue)
		{
			return FromRgb ((byte) red, (byte) green, (byte) blue);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromRgba (byte red, byte green, byte blue, byte alpha)
		{
			return FromRgba (red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromRgba (int red, int green, int blue, int alpha)
		{
			return FromRgba ((byte) red, (byte) green, (byte) blue, (byte) alpha);
		}

		/// <param name="hue">To be added.</param>
		/// <param name="saturation">To be added.</param>
		/// <param name="brightness">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromHsb (nfloat hue, nfloat saturation, nfloat brightness)
		{
			return FromHsba (hue, saturation, brightness, 1.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromHsba (byte hue, byte saturation, byte brightness, byte alpha)
		{
			return FromHsba (hue / 255.0f, saturation / 255.0f, brightness / 255.0f, alpha / 255.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromHsb (byte hue, byte saturation, byte brightness)
		{
			return FromHsba (hue / 255.0f, saturation / 255.0f, brightness / 255.0f, 1.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromHsba (int hue, int saturation, int brightness, int alpha)
		{
			return FromHsba ((byte) hue, (byte) saturation, (byte) brightness, (byte) alpha);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromHsb (int hue, int saturation, int brightness)
		{
			return FromHsb ((byte) hue, (byte) saturation, (byte) brightness);
		}

		/// <param name="red">To be added.</param>
		/// <param name="green">To be added.</param>
		/// <param name="blue">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromDeviceRgb (nfloat red, nfloat green, nfloat blue)
		{
			return FromDeviceRgba (red, green, blue, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceRgb (byte red, byte green, byte blue)
		{
			return FromDeviceRgba (red / 255.0f, green / 255.0f, blue / 255.0f, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceRgb (int red, int green, int blue)
		{
			return FromDeviceRgb ((byte) red, (byte) green, (byte) blue);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceRgba (byte red, byte green, byte blue, byte alpha)
		{
			return FromDeviceRgba (red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceRgba (int red, int green, int blue, int alpha)
		{
			return FromDeviceRgba ((byte) red, (byte) green, (byte) blue, (byte) alpha);
		}

		/// <param name="hue">To be added.</param>
		/// <param name="saturation">To be added.</param>
		/// <param name="brightness">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromDeviceHsb (nfloat hue, nfloat saturation, nfloat brightness)
		{
			return FromDeviceHsba (hue, saturation, brightness, 1.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceHsba (byte hue, byte saturation, byte brightness, byte alpha)
		{
			return FromDeviceHsba (hue / 255.0f, saturation / 255.0f, brightness / 255.0f, alpha / 255.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceHsb (byte hue, byte saturation, byte brightness)
		{
			return FromDeviceHsba (hue / 255.0f, saturation / 255.0f, brightness / 255.0f, 1.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceHsba (int hue, int saturation, int brightness, int alpha)
		{
			return FromDeviceHsba ((byte) hue, (byte) saturation, (byte) brightness, (byte) alpha);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceHsb (int hue, int saturation, int brightness)
		{
			return FromDeviceHsb ((byte) hue, (byte) saturation, (byte) brightness);
		}

		/// <param name="cyan">To be added.</param>
		/// <param name="magenta">To be added.</param>
		/// <param name="yellow">To be added.</param>
		/// <param name="black">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromDeviceCymk (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black)
		{
			return FromDeviceCymka (cyan, magenta, yellow, black, 1.0f);
		}

		/// <param name="cyan">To be added.</param>
		///         <param name="magenta">To be added.</param>
		///         <param name="yellow">To be added.</param>
		///         <param name="black">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceCymka (byte cyan, byte magenta, byte yellow, byte black, byte alpha)
		{
			return FromDeviceCymka (cyan / 255.0f, magenta / 255.0f, yellow / 255.0f, black / 255.0f, alpha / 255.0f);
		}

		/// <param name="cyan">To be added.</param>
		///         <param name="magenta">To be added.</param>
		///         <param name="yellow">To be added.</param>
		///         <param name="black">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceCymk (byte cyan, byte magenta, byte yellow, byte black)
		{
			return FromDeviceCymka (cyan / 255.0f, magenta / 255.0f, yellow / 255.0f, black / 255.0f, 1.0f);
		}

		/// <param name="cyan">To be added.</param>
		///         <param name="magenta">To be added.</param>
		///         <param name="yellow">To be added.</param>
		///         <param name="black">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceCymka (int cyan, int magenta, int yellow, int black, int alpha)
		{
			return FromDeviceCymka ((byte) cyan, (byte) magenta, (byte) yellow, (byte) black, (byte) alpha);
		}

		/// <param name="cyan">To be added.</param>
		///         <param name="magenta">To be added.</param>
		///         <param name="yellow">To be added.</param>
		///         <param name="black">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromDeviceCymk (int cyan, int magenta, int yellow, int black)
		{
			return FromDeviceCymk ((byte) cyan, (byte) magenta, (byte) yellow, (byte) black);
		}

		/// <param name="red">To be added.</param>
		/// <param name="green">To be added.</param>
		/// <param name="blue">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromCalibratedRgb (nfloat red, nfloat green, nfloat blue)
		{
			return FromCalibratedRgba (red, green, blue, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedRgb (byte red, byte green, byte blue)
		{
			return FromCalibratedRgba (red / 255.0f, green / 255.0f, blue / 255.0f, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedRgb (int red, int green, int blue)
		{
			return FromCalibratedRgb ((byte) red, (byte) green, (byte) blue);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedRgba (byte red, byte green, byte blue, byte alpha)
		{
			return FromCalibratedRgba (red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedRgba (int red, int green, int blue, int alpha)
		{
			return FromCalibratedRgba ((byte) red, (byte) green, (byte) blue, (byte) alpha);
		}

		/// <param name="hue">To be added.</param>
		/// <param name="saturation">To be added.</param>
		/// <param name="brightness">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromCalibratedHsb (nfloat hue, nfloat saturation, nfloat brightness)
		{
			return FromCalibratedHsba (hue, saturation, brightness, 1.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedHsba (byte hue, byte saturation, byte brightness, byte alpha)
		{
			return FromCalibratedHsba (hue / 255.0f, saturation / 255.0f, brightness / 255.0f, alpha / 255.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedHsb (byte hue, byte saturation, byte brightness)
		{
			return FromCalibratedHsba (hue / 255.0f, saturation / 255.0f, brightness / 255.0f, 1.0f);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedHsba (int hue, int saturation, int brightness, int alpha)
		{
			return FromCalibratedHsba ((byte) hue, (byte) saturation, (byte) brightness, (byte) alpha);
		}

		/// <param name="hue">To be added.</param>
		///         <param name="saturation">To be added.</param>
		///         <param name="brightness">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSColor FromCalibratedHsb (int hue, int saturation, int brightness)
		{
			return FromCalibratedHsb ((byte) hue, (byte) saturation, (byte) brightness);
		}

		/// <param name="space">To be added.</param>
		/// <param name="components">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSColor FromColorSpace (NSColorSpace space, nfloat []? components)
		{
			if (components is null)
				throw new ArgumentNullException ("components");

			unsafe {
				fixed (nfloat* componentsptr = components) {
					return _FromColorSpace (space, (IntPtr) componentsptr, components.Length);
				}
			}
		}

		/// <param name="components">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public void GetComponents (out nfloat [] components)
		{
			components = new nfloat [(int) ComponentCount];
			unsafe {
				fixed (nfloat* componentsptr = components)
					_GetComponents ((IntPtr) componentsptr);
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			try {
				string name = this.ColorSpaceName;
				if (name == "NSNamedColorSpace")
					return this.LocalizedCatalogNameComponent + "/" + this.LocalizedColorNameComponent;
				if (name == "NSPatternColorSpace")
					return "Pattern Color: " + this.PatternImage.Name;

				StringBuilder sb = new StringBuilder (this.ColorSpace.LocalizedName);
				nfloat [] components;
				this.GetComponents (out components);
				if (components.Length > 0)
					sb.Append ("(" + components [0]);
				for (int i = 1; i < components.Length; i++)
					sb.Append ("," + components [i]);
				sb.Append (")");

				return sb.ToString ();
			} catch {
				//fallback to base method if we have an unexpected condition.
				return base.ToString ();
			}
		}
	}
}
#endif // !__MACCATALYST__
