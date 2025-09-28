package com.unity3d.player;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.webkit.WebView;

public class PrivacyActivity extends Activity implements DialogInterface.OnClickListener {

   // 隐私协议内容
    final String privacyContext =
            "<html><body style='padding:10px; font-size:14px; line-height:1.5;'>" +
            "欢迎使用本游戏，在使用本游戏前，请您充分阅读并理解 " +
            "<a href=\"http://verification.weijinggame.com/weijing/MoLong/MoLongXieYi.txt\">《用户协议》</a>和" +
            "<a href=\"http://verification.weijinggame.com/weijing/MoLong/MoLongYinSi.txt\">《隐私政策》</a>各条;<br><br>" +
            "1. 保护用户隐私是本游戏的一项基本政策，本游戏不会泄露您的个人信息；<br>" +
            "2. 我们会根据您使用的具体功能需要，收集必要的用户信息（如申请设备信息，存储等相关权限）；<br>" +
            "3. 在您同意App隐私政策后，我们将进行集成SDK的初始化工作，会收集您的android_id、Mac地址、IMEI和应用安装列表，以保障App正常数据统计和安全风控；<br>" +
            "4. 为了方便您的查阅，您可以通过“设置”重新查看该协议；<br>" +
            "5. 您可以阅读完整版的隐私保护政策了解我们申请使用相关权限的情况，以及对您个人隐私的保护措施。" +
            "</body></html>";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // 如果已经同意过隐私协议则直接进入Unity Activity
        if (GetPrivacyAccept()) {
            EnterUnityActivity();
            return;
        }
        // 弹出隐私协议对话框
        ShowPrivacyDialog();
    }

    // 显示隐私协议对话框
    private void ShowPrivacyDialog() {
        LayoutInflater inflater = LayoutInflater.from(this);
        View dialogView = inflater.inflate(R.layout.dialog_privacy_scroll, null);

        WebView webView = dialogView.findViewById(R.id.privacy_webview);
        webView.loadDataWithBaseURL(null, privacyContext, "text/html", "utf-8", null);

        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("隐私协议");
        builder.setView(dialogView);
        builder.setCancelable(false);
        builder.setPositiveButton("同意", this);
        builder.setNegativeButton("拒绝", this);
        builder.create().show();
    }

    @Override
    public void onClick(DialogInterface dialogInterface, int i) {
        if (i == AlertDialog.BUTTON_POSITIVE) {
            //点击同意按钮
            SetPrivacyAccept(true);
            EnterUnityActivity();
        } else if (i == AlertDialog.BUTTON_NEGATIVE) {
            //点击拒绝按钮,直接退出App
            finish();
        }
    }

    // 启动Unity Activity
    private void EnterUnityActivity() {
//         Intent unityAct = new Intent();
//         unityAct.setClassName(this, "com.unity3d.player.UnityPlayerActivity");
//         this.startActivity(unityAct);
        Intent unityAct = new Intent(this, com.ChinaBoy.molong.MainActivity.class);
        startActivity(unityAct);
        finish();
    }

    // 本地存储保存同意隐私协议状态
    private void SetPrivacyAccept(boolean accepted) {
        SharedPreferences.Editor prefs = getSharedPreferences("PlayerPrefs", MODE_PRIVATE).edit();
        prefs.putBoolean("PrivacyAcceptedKey", accepted);
        prefs.apply();
    }

    // 获取是否已经同意过
    private boolean GetPrivacyAccept() {
        SharedPreferences prefs = getSharedPreferences("PlayerPrefs", MODE_PRIVATE);
        return prefs.getBoolean("PrivacyAcceptedKey", false);
    }
}
