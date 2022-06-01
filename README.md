# MLSeed
ネットに落ちている簡単なチュートリアルを改造した MLAgents の環境構築済み雛形
- Goalの座標が与えられていたのを、Agent から Ray を飛ばして探すようにした
- Heuristic モード時に画面タッチで操作できるようにした

# Requirement
- Python                                3.7
- ML Agents                             2.1.0-exp.1
- TouchScript                           9.0
- ProGrids                              3.0.3-preview.6
- ProBuilder                            4.5.

# Installation
1. https://github.com/Unity-Technologies/ml-agents/releases/tag/release_19 の最下部からzipをダウンロードして解凍
2. 解答したフォルダを ml-agents に改名して `MLSeed/ml-agents` を上書き
3. コンソールで `MLSeed/ml-agents/` に移動して以下のコマンドを実行<br>
```
$ conda create --name ml-agents python=3.7
$ conda activate ml-agents
$ pip install --upgrade pip 
$ pip install -e ./ml-agents-envs 
$ pip install -e ./ml-agents
```

## Leaning
`MLSeed/ml-agents/` で以下を実行
```
$ mlagents-learn ./config/UserAgents.yaml --run-id= ${run_id}
```

# Note
Unity 2020.3.20f1

# Author 
@Cl2_CHINO