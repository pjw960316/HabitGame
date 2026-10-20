package com.habitgame.notificationpractice;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public final class WaterNotificationReceiver extends BroadcastReceiver
{
    private static final String ACTION_CONFIRM = "com.habitgame.notificationpractice.CONFIRM_WATER";
    private static final String EXTRA_NOTIFICATION_ID = "notification_id";
    private static final String PREFERENCES_NAME = "habit_water_practice";
    private static final String CONFIRMED_AT_KEY = "last_confirmed_at";

    // C#에서 호출합니다. [예]를 누르면 이 수신기를 실행하도록 연결합니다.
    public static void addConfirmButton(Context context, Notification.Builder builder, int notificationId)
    {
        Intent intent = new Intent(context, WaterNotificationReceiver.class);
        intent.setAction(ACTION_CONFIRM);
        intent.putExtra(EXTRA_NOTIFICATION_ID, notificationId);

        PendingIntent pendingIntent = PendingIntent.getBroadcast(context, notificationId, intent, PendingIntent.FLAG_UPDATE_CURRENT | PendingIntent.FLAG_IMMUTABLE);
        builder.addAction(new Notification.Action.Builder(null, "예", pendingIntent).build());
    }

    // [예]를 누르면 Android가 호출합니다. Unity의 실행 여부와 관계없이 처리합니다.
    @Override
    public void onReceive(Context context, Intent intent)
    {
        if (!ACTION_CONFIRM.equals(intent.getAction()))
        {
            return;
        }

        // 앱 전용 저장소에 마지막 클릭 시각 하나를 보관합니다. 값의 단위는 Unix 밀리초입니다.
        context.getSharedPreferences(PREFERENCES_NAME, Context.MODE_PRIVATE).edit().putLong(CONFIRMED_AT_KEY, System.currentTimeMillis()).apply();

        NotificationManager notificationManager = context.getSystemService(NotificationManager.class);
        if (notificationManager != null)
        {
            notificationManager.cancel(intent.getIntExtra(EXTRA_NOTIFICATION_ID, -1));
        }
    }

    // 다음 게임 실행 때 C#에서 호출합니다. 기록이 없으면 0을 반환합니다.
    public static long getLastConfirmedAt(Context context)
    {
        return context.getSharedPreferences(PREFERENCES_NAME, Context.MODE_PRIVATE).getLong(CONFIRMED_AT_KEY, 0L);
    }
}
