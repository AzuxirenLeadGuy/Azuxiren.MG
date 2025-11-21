using Microsoft.Xna.Framework;
namespace Azuxiren.MG;
/// <summary>Contains common physics movement</summary>
public static class Physics
{
	/// <summary>
	/// Updates an object for the given acceleration and friction
	/// </summary>
	/// <param name="velocity">Velocity of the object</param>
	/// <param name="position">Position of the object</param>
	/// <param name="acc">Acceleration acting on the object</param>
	/// <param name="friction">Friction of the surface</param>
	public static void MovePhyObject(
		ref Vector2 velocity,
		ref Vector2 position,
		Vector2? acc = null,
		in float friction = 0
	)
	{
		velocity += (acc ?? default) - (velocity * friction);
		position += velocity;
	}
	/// <summary>
	/// Updates an object for the given acceleration and friction
	/// </summary>
	/// <param name="velocity">Velocity of the object</param>
	/// <param name="position">Position of the object</param>
	/// <param name="acc">Acceleration acting on the object</param>
	/// <param name="friction">Friction of the surface</param>
	public static void MovePhyObject(
		ref Vector3 velocity,
		ref Vector3 position,
		Vector3? acc = null,
		in float friction = 0
	)
	{
		velocity += (acc ?? default) - (velocity * friction);
		position += velocity;
	}
}