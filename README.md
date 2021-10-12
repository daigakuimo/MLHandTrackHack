[![LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](LICENSE.md)
[![Latest GitHub release](https://img.shields.io/github/v/release/bibinba/VRUIPartsCollection)]()
![Unity](https://img.shields.io/badge/-Unity-000000.svg?logo=unity&style=popout)


# MLHandTrackHack

MLHandTrackHackはXR HMDにおける汎用UIの実験プロジェクトです。<br>
このリポジトリではMagicLeapを対象としています。

現在は：<br>
- Revolve Icons<br>
- New Fingers

の2つのUIを開発しています。

開発の経緯、UIの詳細な説明は本リポジトリの前身となったプロジェクト[「Character as an Interface」のサイト](https://hyasssy.tokyo/projects/caai/)
に記載しています。<br>


## Revolve Icons
![RI](https://user-images.githubusercontent.com/36768869/136803392-f68354c5-d9d9-4400-98f7-583338e1aa25.gif)

## New Fingers
![NF](https://user-images.githubusercontent.com/36768869/136804201-836edbd8-b7a9-4981-ada1-b6117101e8ec.gif)


## Getting Started
1. 以下のライブラリをUnityに導入。

    |  Name                | Version |
    | ----                 | ----    |
    |  DOTween             | 1.2.632 |
    |  UniRx               |  7.1.0  |
    |  UniTask             |  2.2.5  |
    |  MagicLeap SDK       |  0.26   |
    |  XR Plugin Manager   |  4.0.1  |

2. [Magic-Leap-Toolkit](https://github.com/magicleap/Magic-Leap-Toolkit-Unity/blob/master/package/MagicLeap-Tools.unitypackage)
をダウンロードし、インポート。

3. [最新のリリース](https://github.com/daigakuimo/MLHandTrackHack/releases)
から[MLHandTrackHack.unitypackage](https://github.com/daigakuimo/MLHandTrackHack/releases/latest/download/MLHandTrackHack_v1.0.unitypackage)
をダウンロードし、インポート。

4. UnityのプロジェクトでApp/RevolveIcons or NewFingers/ExampleフォルダにExampleシーンがあります。

## Usage
### Revolve Icons
1. 設定したいイベントのアイコン画像を用意。<br>
   
2. `App/RevolveIcons/Prefabs/Icons/IconOrigin`プレハブの子要素`IconImage`にアイコン画像を設定。<br>
   ![RI_usage_2](https://user-images.githubusercontent.com/36768869/136963295-6dae9b0f-478f-4618-9d49-87a639bc666c.png)


3. `IconOrigin`プレハブにある`IconBehavior`コンポーネントの`OnFistEvent`にアイコンが選択された状態でFist(握る)ジェスチャした際のイベントを設定。<br>
   ![RI_usage_3](https://user-images.githubusercontent.com/36768869/136963307-33def44d-2f2b-4ff5-bc6b-af65158699b6.png)


4. `RevolveIcons`プレハブの子要素`IconsRoot`にある`CircleObjectCreator`コンポーネントの`Icons`に作ったアイコンをアタッチ。<br>
   ![RI_usage_4](https://user-images.githubusercontent.com/36768869/136963322-754fe1d8-75ea-46f8-b241-7471df428e9d.png)


5. 1~4までを作りたいアイコンの数繰り返す。<br>


6. アイコンをすべてアタッチできたら、`IconsRoot`の`CircleObjectCreator`をアクティブにすると、アタッチしたアイコンが円形に配置される。円形に配置されたら`CircleObjectCreator`を非アクティブにしておく。<br>
   ![RI_usage_6](https://user-images.githubusercontent.com/36768869/136963334-0aadafc5-a4e9-4e8f-a9aa-30ab5c5f5992.png)


7. `RevolveIcons`の`RevolveIconsController`の`IconNum`に設定したアイコンの数を入力。<br>
   ![RI_usage_7](https://user-images.githubusercontent.com/36768869/136963343-af5510db-a17b-4347-b0e3-08b5aa7b3aa5.png)


8. シーン上のオブジェクトに`RevolveIconsCreator`コンポーネントを追加し、`RevolveIconsPrefab`に7までに作ったプレハブをアタッチ。<br>
   ![RI_usage_8](https://user-images.githubusercontent.com/36768869/136963352-177dcb3b-9fd5-48fa-b769-b09741ad7317.png)


9. ビルドまたはZero Iterationで確認。<br>



### NewFingers

1. `MLHandTrackHack.NewFingers.NewFingerApp`を継承したスクリプトを作成し、`StartApp`メソッドに指をくっつけた時の処理を記述。
   ![NF_usage_1](https://user-images.githubusercontent.com/36768869/136974693-1de53b91-8dca-46f2-9e26-638e50859955.png)

2. 1で作ったスクリプトを`App/NewFingers/Prefabs/NewFingers`プレハブに追加。
   

3. `App/NewFingers/Prefabs/NewFingers`プレハブの子要素`IndexObj`(人差し指)→`FingerTipController`→`NewFingerApp`に2で追加したスクリプトをアタッチ。
   ![NF_usage_2](https://user-images.githubusercontent.com/36768869/136974708-e612cb74-eb47-42d8-9a14-3a16037cccb9.png)

4. 1~3を他の指にも設定。<br>
   

5. ビルドまたはZero Iterationで確認。<br>


## Author
- Yuto Hayashi
- Ayato Enami
- Mitibumi Ito

## License
- This asset is under MIT License.
