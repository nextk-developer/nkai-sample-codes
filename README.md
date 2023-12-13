# **nkai-sample-codes README**
NK에서 제공하는 서비스 사용을 위한 예제 샘플 코드

# **NKClientQuickSample QuickStarts**
## **구성**
<img src="doc/flow.png" width="600px" height="150px" title="검출 이벤트 결과" alt="flowImage"></img><br/>
***

## **사전 준비**
* .NET SDK 7.0
  - https://dotnet.microsoft.com/ko-kr/download/dotnet/7.0
* Visual Studio 2022 설치
  - https://visualstudio.microsoft.com/ko/vs/
* NK Package
  - NKProcess 설치 및 실행
    - 지원 GPU : Nvidia 30xx Seriese, T4
    - 최신 드라이버 설치(https://www.nvidia.co.kr/Download/index.aspx?lang=en)
    - Support Cuda Gpus(https://en.wikipedia.org/wiki/CUDA)
* ffmpeg5 dll 설치
  - 솔루션 경로/NKAPISample 폴더에 dlls 폴더 생성
  - 생성한 dlls 폴더에 ffmpeg 폴더 생성
  - 생성된 [솔루션 경로/NKAPISample/dlls/ffmpeg] 폴더에 ffmpeg5 dll 붙여넣기
	- avcodec-59.dll
	- avformat-59.dll
	- avutil-57.dll
	- swresample-4.dll
	- swscale-6.dll
* NKAPISample 솔루션 빌드

## **직접 API 요청시**
### **노드 등록**
**Request**

POST /v2/va/create-computing-node
```
{
  "host": "nextk.synology.me",
  "nodeName": "TEST_COMPUTE_NODE"
}
```
**Response**
```
//success
{
  "nodeId": "c7af8a06cd84ce2d",
  "productVersion": "1.0.9.10",
  "releaseDate": "2023.10.11",
  "code": 0,
  "message": "SUCCESS"
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
  "nodeId": "c7af8a06cd84ce2d",
  "channelId": "",
  "channelName": "NextK Channel",
  "inputUrl": "rtsp://nextkMedia.synology.me/vod/line1",
  "inputUrlSub": "rtsp://nextkMedia.synology.me/vod/line1",
  "inputType": 0,
  "groupName": "NextK Group",
  "description": "NextK",
  "autoTimeout": true
}
```
**Response**
```
//success
{
  "channelId": "9c3ea862363a940a",
  "mediaServerUrl": "rtsp://nextk.synology.me:8554/live/main/9c3ea862363a940a",
  "mediaServerUrlSub": "",
  "sourceType": "NK-Edge",
  "code": 0,
  "message": "SUCCESS"
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
  "nodeId": "c7af8a06cd84ce2d",
  "channelId": "9c3ea862363a940a",
  "roiId": null,
  "name": "ROI",
  "roiType": 5,
  "description": null,
  "stayTime": 0.0,
  "numberOf": 0,
  "eventType": "AllDetect",
  "feature": 0,
  "roiDots": [
    {
      "x": 0.0,
      "y": 0.0
    },
    {
      "x": 1.0,
      "y": 0.0
    },
    {
      "x": 1.0,
      "y": 1.0
    },
    {
      "x": 0.0,
      "y": 1.0
    }
  ],
  "roiDotsSub": [
    {
      "x": 0.0,
      "y": 0.0
    },
    {
      "x": 1.0,
      "y": 0.0
    },
    {
      "x": 1.0,
      "y": 1.0
    },
    {
      "x": 0.0,
      "y": 1.0
    }
  ],
  "eventFilter": {
    "minDetectSize": {
      "width": 0.0,
      "height": 0.0
    },
    "maxDetectSize": {
      "width": 1.0,
      "height": 1.0
    },
    "objectsTarget": [
      0,
      6,
      7,
      8,
      1002,
      1004,
      1001,
      1003,
      600,
      601,
      602
    ]
  },
  "objectTypes": null,
  "roiNumber": 0,
  "params": null
}
```
**Response**
```
//success
{
  "roiId": "f4c7c550c978642a",
  "code": 0,
  "message": "SUCCESS"
}
//fail
{
  "message": "Not Found ComputingNode"
  "code": 330,
}
```
<!-- ### 비디오 분석 시작-->

## **Sample UI 사용시**
1) *ComptuteNode 하위 정보 입력
2) *Channel RTSP URL 입력
3) *Select API 포맷 이용 (아래 순서로 요청)
   - [Computing Node] - [Create] 버튼 클릭하여 노드 생성
   - [Channel] - [Create] 버튼 클릭하여 채널 생성
   - [Schedule] - [Add] 버튼 클릭하여 매일 1시간마다 분석 스케줄 지정(기본값)
   - [RoI] - [Object] 분석 대상 선택 - [Event] 분석 이벤트 선택 - [Range] 영역 타입 클릭 - 우측 [Streaming] 에서 화면 클릭하여 영역 지정
     - [Select All] : 전체 영역
	 - [Rectangle] : 사각 영역(화면 내에서 두 점을 클릭하여 사각 영역 지정)
	 - [Polygon] : 다각 영역(화면 내에서 n개의 점을 클릭하여 다각 영역 지정)
	 - [Line] : 선 영역(화면 내에서 두 점을 클릭하여 하나의 선 영역 지정)
	 - [Multi Line] : 다중 선 영역(화면 내에서 두 점씩 n번 클릭하여 다중 선 영역 지정)
   - [VA] -[Start] 버튼 클릭하여 분석 시작

## **분석 결과 확인**
영상 내 이벤트 발생시 결과 영상 Annotation 및 Text 출력

<img src="doc/metadata.png" width="640px" height="480px" title="검출 이벤트 결과" alt="metaImage"></img><br/>


* Sample에서 직접 영상을 수신하여 Annotaion 출력 박스보다 영상이 딜레이 될 수 있음
