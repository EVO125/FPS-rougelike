using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 工具类
/// </summary>
public static class Tool
{
    public static int currCherk = 1;

    /// <summary>
    /// 切换场景
    /// </summary>
    public static void ChangeScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }
}
