using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class CircleObjectCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> icons; // 生成するオブジェクト

    [SerializeField]
    private float radius = 5f; // 半径

    [SerializeField]
    private float repeat = 2f; // 何周期するか
    
    private List<GameObject> Objects = new List<GameObject>();

    void Update()
    {
        var oneCycle = 2.0f * Mathf.PI; // sin の周期は 2π
        Objects.ForEach(DestroyImmediate);

        for (var i = 0; i < icons.Count; ++i)
        {

            var point = ((float)i / icons.Count) * oneCycle; // 周期の位置 (1.0 = 100% の時 2π となる)
            var repeatPoint = point * repeat; // 繰り返し位置

            var x = Mathf.Cos(repeatPoint) * radius;
            var y = Mathf.Sin(repeatPoint) * radius;

            var position = new Vector3(y, 0, x);

            Objects.Add(Instantiate(
                    icons[i],
                position,
                Quaternion.identity,
                transform
                )
            );

        }
    }
}
