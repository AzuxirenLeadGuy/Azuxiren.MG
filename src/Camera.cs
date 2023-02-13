using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG;
/// <summary>A simple 2D camera implementation with zooming and rotation</summary>
public class Camera2D
{
	/// <summary>The position of the top left rectangle of camera source</summary>
	/// <value>Vector2 Value of position</value>
	public Vector2 Position { get; set; }
	/// <summary>The zoom for camera</summary>
	/// <value>positive float Value of Zoom in (0, inf)</value>
	public float Zoom { get; set; }
	/// <summary>The camera's rotation in the Z axis</summary>
	/// <value>Rotation angle in radians</value>
	public float Rotation { get; set; }
	private Matrix _transform;
	private readonly GraphicsDevice _graphicsDevice;
	/// <summary>Camera constructor</summary>
	/// <param name="graphicsDevice">The graphicsdevice to obtain the world coordinates from</param>
	public Camera2D(GraphicsDevice graphicsDevice)
	{
		_graphicsDevice = graphicsDevice;
		Zoom = 1f;
		Rotation = 0;
		Position = new(0, 0);
	}
	/// <summary>With the given values, update the transform matrix for the camera</summary>
	public void Update()
	{
		var viewport = _graphicsDevice.Viewport;
		_transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
					 Matrix.CreateRotationZ(Rotation) *
					 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
					 Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
	}
	/// <summary>Returns the transform matrix for the camera</summary>
	/// <returns>Transfrom matrix</returns>
	public Matrix GetTransform() => _transform;
}