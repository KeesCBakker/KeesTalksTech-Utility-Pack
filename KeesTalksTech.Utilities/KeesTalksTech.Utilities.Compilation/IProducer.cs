namespace KeesTalksTech.Utilities.Compilation
{
	/// <summary>
	/// Indicates the object implements an object producer.
	/// </summary>
	public interface IProducer
	{
		/// <summary>
		/// Runs the producer.
		/// </summary>
		/// <returns>The result.</returns>
		object Run();
	}
}
