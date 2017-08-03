# Restaurant_Sys
Restaurant order system

[狀態]:

1. 輪播(影片廣告以及店家促銷或形象宣傳資訊)

(1) 用提供的API去撈廣告資料, 如撈不到則用上次的, 撈的到則覆蓋, 都沒有則報錯

(2) 無限循環直到有人點擊螢幕

2. 點餐
在輪播中點螢幕隨即進入點餐狀態

(1) 記錄點擊者的性別＆年齡

(2) 用提供的API去撈餐點資料

(3) 將(2)的資料顯示在UI上以供點餐

(4) 有語音觸發小遊戲等(不一定有這步驟)

(5) 點餐結束送出點餐結果並進入輪播狀態



[功能]:

1. 輪播廣告資料(影片or圖片)
    
    a. 抓取廣告資料 >> 用HttpWebRequest送POST 然後回來資料用Newtonsoft.Json 解析
    
    b. 輪播廣告資料 >> 用picturebox and Emgu完成
2. 人流分析
3. 點餐
    
    a. 抓取餐點資料 >> 用HttpWebRequest送POST 然後回來資料用Newtonsoft.Json 解析
    
    b. 點餐UI操作
    
    c. 人流分析(取得點餐當下的客戶個數與其性別&年齡) >> 可用google or AWS
    
    d. 通知伺服器點餐結果
    
    e. 語音辨識 >> 可用google or AWS
    
    f. 小遊戲
    
4. 登入功能
