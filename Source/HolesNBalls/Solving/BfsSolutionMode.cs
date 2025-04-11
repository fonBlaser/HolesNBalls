namespace HolesNBalls.Solving;

/// <summary>
/// The mode for BFS solver specifying the search depth and the type of results to include.
/// </summary>
public enum BfsSolutionMode
{
    /// <summary>
    /// The result will include the only first win found.
    /// The result will be empty if no wins found.
    /// </summary>
    FirstWin,

    /// <summary>
    /// The result will include all wins found on the same depth.
    /// The result will be empty if no wins found.
    /// </summary>
    IncludeWinsOnSameDepth,

    /// <summary>
    /// The result will include all wins found on the all depth.
    /// The result will be empty if no wins found.
    /// </summary>
    IncludeWinsOnAllDepths,

    /// <summary>
    /// The result will include all wins, loses and unresolvables found on the same depth.
    /// The result will not be empty.
    /// </summary>
    All
}