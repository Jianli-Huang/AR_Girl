# [Cinemachine](http://www.cinemachineimagery.com/)の導入

既存のカメラアニメーションを取り除く。
Timelineからカメラ関連のトラックをミュートにして、CameraSwitcherからDOF以外の機能を削除する。
2カ所止めれば動かなくなる。

```cs
    void Update()
    {
#if false
        // Update the follow point with the exponential easing function.
        var param = Mathf.Exp(-rotationSpeed * Time.deltaTime);
        followPoint = Vector3.Lerp(target.position, followPoint, param);

        // Look at the follow point.
        transform.LookAt(followPoint);
#endif    

    public void ChangePosition(Transform destination, bool forceStable = false)
    {
        return;
```

再生して初期位置にカメラが静止することを確認する。

* [Unity2017のCinemachineをやってみた](http://blog.applibot.co.jp/blog/2017/07/31/unity2017-cinemachine/)

AssetStoreからImport。
https://www.assetstore.unity3d.com/en/#!/content/79898


CinemachineメニューからCreateVirtualCamera。
すると・・・

すごい勢いでエラがーでる。

```
Assertion failed: Screen position out of view frustum (screen pos 1024.000000, 0.000000, 5000.000000) (Camera rect 0 0 1024 1024)
```

VisualizerにアタッチされているMirrorReflectionと相性が悪いでござる。
MirrorReflectionを無効にした。後でPostProcessingStackのものと置き換えてみよう。

CameraSwitcherで使っていたCameraPointにいくつかCinemachineのVirtualCameraを設置してみる。
とりあえず、Close-up, Close-up2, Finaleの３つを設置。
Close-up2をMiddleに名前変える。
FinaleをLongに名前変える。
TimelineにCinemachineのトラックを作る。
元のカメラのタイムラインのTimelineのClose-up, Close-up2を設定していた時間にクリップを追加してそれぞれのバーチャルカメラを設定、余ったところにクリップを作って全部Longを設定。

再生してみる。タイムラインを操作してみるとカメラの動きをシークできるようになった。
CameraSwitcherに追加したDOF調整が動かなくなったので取り除いて新しいスクリプトを作る。

```cs
using UnityEngine;
using UnityEngine.PostProcessing;

public class AutoFocus : MonoBehaviour
{
    [SerializeField]
    PostProcessingBehaviour m_postprocessing;

    [SerializeField]
    GameObject m_target;

    private void Reset()
    {
        m_postprocessing = GetComponent<PostProcessingBehaviour>();
    }

    void Update()
    {
        var settings = m_postprocessing.profile.depthOfField.settings;
        m_postprocessing.profile.depthOfField.settings = new DepthOfFieldModel.Settings
        {
            focusDistance = (m_target.transform.position - transform.position).magnitude,
            aperture = settings.aperture,
            focalLength = settings.focalLength,
            kernelSize = settings.kernelSize,
            useCameraFov = settings.useCameraFov,
        };
    }
}
```

# 後でカメラワーク考える

* https://pixta.jp/channel/?p=15795

