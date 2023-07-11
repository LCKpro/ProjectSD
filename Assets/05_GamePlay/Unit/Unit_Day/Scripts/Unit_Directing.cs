using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Unit_Directing : MonoBehaviour
{
    public Animator anim_Player;
    public Animator anim_Unit_0001;
    public Animator anim_Unit_0002;
    public Animator anim_Unit_0003;
    public Animator anim_Unit_0004;

    void Start()
    {
        anim_Player.SetInteger("animation", 34);
        anim_Unit_0001.SetInteger("animation", 34);
        anim_Unit_0002.SetInteger("animation", 34);
        anim_Unit_0003.SetInteger("animation", 34);
        anim_Unit_0004.SetInteger("animation", 34);
        Invoke("SetAnimation", 1.5f);
    }


    public void SetAnimation()
    {
        UniTask.Create(async () =>
        {
            try
            {
                anim_Unit_0001.SetInteger("animation", 42);

                await UniTask.Delay(TimeSpan.FromSeconds(0.25f)).SuppressCancellationThrow();

                anim_Unit_0002.SetInteger("animation", 53);

                await UniTask.Delay(TimeSpan.FromSeconds(0.25f)).SuppressCancellationThrow();

                anim_Unit_0003.SetInteger("animation", 50);

                await UniTask.Delay(TimeSpan.FromSeconds(0.25f)).SuppressCancellationThrow();

                anim_Unit_0004.SetInteger("animation", 52);

                await UniTask.Delay(TimeSpan.FromSeconds(0.25f)).SuppressCancellationThrow();

                anim_Player.SetInteger("animation", 13);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        });
    }
}
