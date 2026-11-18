package com.habitgame.notificationpractice;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;

import java.time.Instant;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.util.Collections;
import java.util.HashSet;
import java.util.Set;

public final class WaterNotificationReceiver extends BroadcastReceiver
{
    private static final String ACTION_CONFIRM = "com.habitgame.notificationpractice.CONFIRM_WATER";
    private static final String EXTRA_NOTIFICATION_ID = "notification_id";
    private static final String PREFERENCES_NAME = "habit_water_practice";
    private static final String CONFIRMED_AT_KEY = "last_confirmed_at";
    private static final String CONFIRMED_DATES_KEY = "confirmed_dates";

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

        saveConfirmedDate(context);

        NotificationManager notificationManager = context.getSystemService(NotificationManager.class);
        if (notificationManager != null)
        {
            notificationManager.cancel(intent.getIntExtra(EXTRA_NOTIFICATION_ID, -1));
        }
    }

    private static void saveConfirmedDate(Context context)
    {
        SharedPreferences preferences = context.getSharedPreferences(PREFERENCES_NAME, Context.MODE_PRIVATE);
        long confirmedAt = System.currentTimeMillis();
        Set<String> confirmedDates = new HashSet<>(preferences.getStringSet(CONFIRMED_DATES_KEY, Collections.emptySet()));

        long previousConfirmedAt = preferences.getLong(CONFIRMED_AT_KEY, 0L);
        if (confirmedDates.isEmpty() && previousConfirmedAt > 0L)
        {
            confirmedDates.add(toLocalDateKey(previousConfirmedAt));
        }

        confirmedDates.add(toLocalDateKey(confirmedAt));
        preferences.edit().putStringSet(CONFIRMED_DATES_KEY, confirmedDates).putLong(CONFIRMED_AT_KEY, confirmedAt).apply();
    }

    private static String toLocalDateKey(long confirmedAt)
    {
        return Instant.ofEpochMilli(confirmedAt).atZone(ZoneId.systemDefault()).toLocalDate().format(DateTimeFormatter.BASIC_ISO_DATE);
    }

    // 다음 게임 실행 때 C#에서 호출합니다. 기록이 없으면 0을 반환합니다.
    public static long getLastConfirmedAt(Context context)
    {
        return context.getSharedPreferences(PREFERENCES_NAME, Context.MODE_PRIVATE).getLong(CONFIRMED_AT_KEY, 0L);
    }
}
