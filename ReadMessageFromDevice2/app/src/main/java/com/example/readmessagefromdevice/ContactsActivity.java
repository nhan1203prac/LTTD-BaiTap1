package com.example.readmessagefromdevice;

import android.Manifest;
import android.annotation.SuppressLint;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import java.util.ArrayList;
import java.util.HashSet;

public class ContactsActivity extends AppCompatActivity {
    private static final int REQUEST_CODE_READ_SMS = 100;
    ListView listViewContacts;
    ArrayList<String> contactsList;
    ArrayAdapter<String> adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_contacts);

        listViewContacts = findViewById(R.id.listViewContacts);
        contactsList = new ArrayList<>();

        adapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, contactsList);
        listViewContacts.setAdapter(adapter);

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_SMS)
                != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.READ_SMS}, REQUEST_CODE_READ_SMS);
        } else {
            loadContacts();
        }

        listViewContacts.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String contact = contactsList.get(position);
                Intent intent = new Intent(ContactsActivity.this, MessagesActivity.class);
                intent.putExtra("contact", contact);
                startActivity(intent);
            }
        });
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == REQUEST_CODE_READ_SMS) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                loadContacts();
            } else {
                Toast.makeText(this, "Permission denied to read SMS", Toast.LENGTH_SHORT).show();
            }
        }
    }

    private void loadContacts() {
        Uri uri = Uri.parse("content://sms/");
        Cursor cursor = getContentResolver().query(uri, null, null, null, null);

        if (cursor != null) {
            HashSet<String> uniqueContacts = new HashSet<>();
            while (cursor.moveToNext()) {
                @SuppressLint("Range") String address = cursor.getString(cursor.getColumnIndex("address"));
                if (address != null && uniqueContacts.add(address)) {
                    contactsList.add(address);
                }
            }
            cursor.close();
        }

        adapter.notifyDataSetChanged();
    }


}
