package NaverAPI;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.Socket;

public class MySocketClient {
		
//	public static void main(String[] args) {
//
//        Socket socket = null;
//
//        try {
//            socket = new Socket();
//            System.out.println("[연결 요청]");
//            socket.connect(new InetSocketAddress("192.168.20.18", 5001));
//            System.out.println("[연결 성공]");
//           
//            byte[] bytes = null;
//            String message = null;
//
//            OutputStream os = socket.getOutputStream();
//            message = "Hello Server, I'm Client.";
//            bytes = message.getBytes("UTF-8");
//            os.write(bytes);
//            os.flush();
//            System.out.println("[데이터 보내기 성공]");
//           
//            InputStream is = socket.getInputStream();
//            bytes = new byte[100];
//            int readByteCount = is.read(bytes);
//            message = new String(bytes,0,readByteCount,"UTF-8");
//            System.out.println("[데이터 받기 성공] " + message);
//           
//            os.close();
//            is.close();
//           
//        } catch (Exception e) {
//            e.printStackTrace();
//        }
//       
//        if (!socket.isClosed()) {
//            try {
//                socket.close();
//            } catch (Exception e) {
//                e.printStackTrace();
//            }
//        }
//
//    }
	
	public static void main(String[] args) { 
		try {
			Socket socket = new Socket("192.168.20.18",1222);
			System.out.println("소켓 서버 접속완료");
			
			//Client -> Server로 전송
			OutputStream out = socket.getOutputStream();
			PrintWriter pr = new PrintWriter(out,true);
			
			pr.println("CLIENT TO SERVER");
			//socket.close(); //소켓 종료
			
			//서버가 보낸 메시지 읽기
			InputStream input = socket.getInputStream();
			BufferedReader br = new BufferedReader(new InputStreamReader(input));
			
			System.out.println(br.readLine()); //서버로부터 온 메세지 확인
			
			System.out.println("CLIENT SOCKET CLOSE");
			socket.close(); //소켓 종료
		}catch (Exception e) {
			e.printStackTrace();
			System.out.println("err");
		}
	}

}
