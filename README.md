# **nkai-sample-codes README**
NK에서 제공하는 서비스 사용을 위한 예제 샘플 코드

# **NKClientQuickSample QuickStarts**
## **구성**
<img src="doc/flow.png" width="600px" height="150px" title="검출 이벤트 결과" alt="metaImage"></img><br/>
***
## **사전 준비**
* NK Package
  - NKAPIGateWay 설치 및 실행
  - NKProcess 설치 및 실행
    - 지원 GPU : Nvidia 30xx Seriese, T4
    - 최신 드라이버 설치(https://www.nvidia.co.kr/Download/index.aspx?lang=en)
    - Support Cuda Gpus(https://en.wikipedia.org/wiki/CUDA)
* NKClientQuickSample 솔루션 빌드
## **직접 API 요청시**
### **노드 등록**
**Request**

POST /v2/va/create-computing-node
```
{
  "host": "nextk.synology.me",
  "httpPort": 8880,
  "rpcPort": 33300,
  "nodeName": "TEST_COMPUTE_NODE"
}
```
**Response**
```
//success
{
  "nodeId": "6fac2e0e",
  "code": 0
}
//fail
{
  "message": "Not Found Computing Node",
  "code": 400
}
//already create
{
  "nodeId": "6fac2e0e",
  "message": "ComputeNode that already exists",
  "code": 300
}
```
### **채널 등록**
**Request**

POST /v2/va/register-channel
```
{
  "nodeId": "6fac2e0e",
  "channelName": "TEST_CHANNEL_NAME",
  "inputUri": "rtsp://admin:enter2424@192.168.0.71/stream1"
}
```
**Response**
```
//success
{
  "channelId": "cdc5eaaf",
  "code": 0
}
//fail
{
  "message": "Not Found Computing Node",
  "code": 400
}
```
### **ROI 생성**
**Request**

POST /v2/va/create-roi
```
{
  "nodeId": "6fac211e0e",
  "channelId": "cdc5eaaf",
  "roiName": "ROI_NAME",
  "description": "Loitering Event",
  "roiDots": [
    {
      "X": 0.0,
      "Y": 0.0
    },
    {
      "X": 1.0,
      "Y": 0.0
    },
    {
      "X": 1.0,
      "Y": 1.0
    },
    {
      "X": 0.0,
      "Y": 1.0
    }
  ]
}
```
**Response**
```
//success
{
  "roiId": "adaaf82f",
  "code": 0
}
//fail
{
  "message": "Not Found ComputingNode"
  "code": 330,
}
```
<!-- ### 비디오 분석 시작-->

## **Sample UI 사용시**
1) Setting > BaseUri 입력
2) *ComptuteNode 하위 정보 입력
3) *Select API 포맷 이용 (아래 순서로 요청)
   - ComputeNode Add 선택 -->  API Send 버튼
   - Channel Add 선택 --> API Send 버튼
   - Roi Add 선택 --> API Send 버튼

## **분석 결과 확인**
영상 내 이벤트 발생시 결과 영상 Annotation 및 Text 출력

<img src="doc/metadata.png" width="640px" height="480px" title="검출 이벤트 결과" alt="metaImage"></img><br/>


* Sample에서 직접 영상을 수신하여 Annotaion 출력 박스보다 영상이 딜레이 될 수 있음
