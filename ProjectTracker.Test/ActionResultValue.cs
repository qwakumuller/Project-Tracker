using Microsoft.AspNetCore.Mvc;

namespace ProjectTracker.Test;

/// <summary>
/// The main idea of this class is to provide us an extension method that parses the controller returned object which is IActionResult.
/// </summary>
public static class ActionResultValue
{
    /// <summary>
    /// By calling this method on the IActionResult object, you get the actual object such as Project
    /// An example, 
    /// var projectResult = await _controller.DeleteProject(61);
    /// var result = projectResult.GetValue<Project>();
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    /// <param name="_self">The IActionResult object</param>
    /// <returns></returns>
    public static T? GetValue<T>(this IActionResult _self) where T : class
    {
        if (_self is OkResult)
            return (T?)Activator.CreateInstance(typeof(T));

        var result = _self as ObjectResult;
        if (result!.StatusCode != 200 && result!.StatusCode != 201)
            return default;
        return result?.Value is null ? default : (T)result.Value;
    }
}
