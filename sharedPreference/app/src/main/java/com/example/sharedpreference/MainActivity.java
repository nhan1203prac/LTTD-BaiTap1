package com.example.sharedpreference;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {

    private TextView textViewTitle, textViewMsv, textViewTen, textViewKhoa, textViewLhp;
    private EditText editTextMsv, editTextTen, editTextKhoa, editTextLhp;
    private CheckBox checkBoxLuuThongTin;
    private SharedPreferences sharedPreferences;
    private Button btnxacnhan;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        textViewTitle = findViewById(R.id.textView);
        textViewMsv = findViewById(R.id.label_msv);
        editTextMsv = findViewById(R.id.editText_msv);
        textViewTen = findViewById(R.id.label_ten);
        editTextTen = findViewById(R.id.editText_ten);
        textViewKhoa = findViewById(R.id.label_khoa);
        editTextKhoa = findViewById(R.id.editText_khoa);
        textViewLhp = findViewById(R.id.label_lhp);
        editTextLhp = findViewById(R.id.editText_lhp);
        checkBoxLuuThongTin = findViewById(R.id.checkbox);
        btnxacnhan = (Button) findViewById(R.id.btnxacnhan);

        sharedPreferences = getSharedPreferences("data",MODE_PRIVATE);

        editTextMsv.setText(sharedPreferences.getString("msv",""));
        editTextTen.setText(sharedPreferences.getString("ten",""));
        editTextKhoa.setText(sharedPreferences.getString("khoa",""));
        editTextLhp.setText(sharedPreferences.getString("lhp",""));
        checkBoxLuuThongTin.setChecked(sharedPreferences.getBoolean("checked",false));


        btnxacnhan.setOnClickListener(v->{
            if(checkBoxLuuThongTin.isChecked()){
                SharedPreferences.Editor editor = sharedPreferences.edit();
                editor.putString("msv",editTextMsv.getText().toString());
                editor.putString("ten",editTextTen.getText().toString());
                editor.putString("khoa",editTextKhoa.getText().toString());
                editor.putString("lhp",editTextLhp.getText().toString());
                editor.putBoolean("checked",true);
                editor.commit();


            }
            else {
                SharedPreferences.Editor editor =  sharedPreferences.edit();
                editor.clear();
                editor.commit();
            }
            Toast.makeText(this,"Xác nhận thành công",Toast.LENGTH_LONG);
        });


    }
}