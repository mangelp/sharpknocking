
using System;

namespace SharpKnocking.Common
{
	
	/// <summary>
	/// Operaciones que permiten interactuar con cada uno de las clases que
	/// trabajan en varios hilos pero que pertecen a un sólo proceso.
	/// </summary> 
	public interface IDaemonProcessUnit: IDisposable
	{
	    /// <summary>.
	    /// This starts the procesing. This method doesn't ends until the
	    /// kill method is executed.
	    /// </summary>
	    void Run ();
	    /// <summary>
	    /// Returns true if the daemon is running. False if not.
	    /// </summary>
	    bool Running { get; }
	    /// <summary>
	    /// If the daemon is running and it is stopped this starts it.
	    /// </summary>
	    void Start ();
	    /// <summary>
	    /// If the daemon is running and it is'n stopped this stops it.
	    /// </summary>
	    void Stop ();
	    /// <summary>
	    /// Returns true if the daemon is running and stoped. False if not.
	    /// </summary>
	    bool Stopped { get;}
	}
}
