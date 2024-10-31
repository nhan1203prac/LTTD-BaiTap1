package com.example.readmessagefromdevice;

import android.annotation.SuppressLint;
import android.os.Bundle;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import java.util.ArrayList;

public class MessagesActivity extends AppCompatActivity {
    ListView listViewMessages;
    ArrayList<String> messagesList;
    ArrayAdapter<String> adapter;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_messages);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });listViewMessages = findViewById(R.id.listViewMessages);
        messagesList = new ArrayList<>();

        adapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, messagesList);
        listViewMessages.setAdapter(adapter);

        String contact = getIntent().getStringExtra("contact");
        loadMessages(contact);
    }

    private void loadMessages(String contact) {
        Uri uri = Uri.parse("content://sms/");
        Cursor cursor = getContentResolver().query(uri, null, "address=?", new String[]{contact}, null);

        if (cursor != null) {
            while (cursor.moveToNext()) {
                @SuppressLint("Range") String body = cursor.getString(cursor.getColumnIndex("body"));
                messagesList.add(body);
            }
            cursor.close();
        }

        adapter.notifyDataSetChanged();
    }
}