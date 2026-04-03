using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Components;
/// <summary>A simple 2D camera implementation with zooming and rotation</summary>
public struct Camera2D
{
	private Vector2 _postition;
	private float _zoom;
	private float _rotation;
	private Rectangle _viewport;
	/// <summary>The origin point for the camera view</summary>
	public Vector2 Position
	{
		readonly get => _postition;
		set
		{
			if (_postition == value) return;
			_postition = value;
			UpdateTransform();
		}
	}
	/// <summary>The rotation (from origin) for the camera view</summary>
	public float Rotation
	{
		readonly get => _rotation;
		set
		{
			if (_rotation == value) return;
			_rotation = value;
			UpdateTransform();
		}
	}
	/// <summary>The zoom/scale value for the camera view</summary>
	public float Zoom
	{
		readonly get => _zoom;
		set
		{
			if (_zoom == value) return;
			_zoom = value;
			UpdateTransform();
		}
	}
	/// <summary>The camera destination view on screen</summary>
	public Rectangle Viewport
	{
		readonly get => _viewport;
		set
		{
			if (_viewport == value) return;
			_viewport = value;
			UpdateTransform();
		}
	}
	/// <summary>The calculated transformation matrix for the camera</summary>
	public Matrix Transform { get; private set; }
	/// <summary>A 2DCamera constructor</summary>
	/// <param name="viewport">The viewport to obtain the world coordinates from</param>
	public Camera2D(Rectangle viewport)
	{
		_postition = viewport.Center.ToVector2();
		_zoom = 1;
		_rotation = 0;
		_viewport = viewport;
		UpdateTransform();
	}
	private void UpdateTransform()
	{
		Transform = Matrix.CreateTranslation(new(-_postition.X, -_postition.Y, 0));
		Transform *= Matrix.CreateRotationZ(_rotation);
		Transform *= Matrix.CreateScale(_zoom, _zoom, 1);
		Transform *= Matrix.CreateTranslation(new(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
	}
	/// <summary>With the given values, update the transform matrix for the camera</summary>
	public void SetValues(
		Vector2? new_position = null,
		float? new_zoom = null,
		float? new_rotation = null,
		Rectangle? new_viewport = null
	)
	{
		Vector2 position = new_position ?? _postition;
		float zoom = new_zoom ?? _zoom;
		float rotation = new_rotation ?? _rotation;
		Rectangle viewport = new_viewport ?? _viewport;
		if (
			position == _postition &&
			zoom == _zoom &&
			rotation == _rotation &&
			viewport == _viewport
		) return;
		_postition = position;
		_zoom = zoom;
		_rotation = rotation;
		_viewport = viewport;
		UpdateTransform();
	}
}