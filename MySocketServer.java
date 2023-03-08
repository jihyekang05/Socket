package NaverAPI;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;

public class MySocketServer {

	public static void main(String[] args) {
        ServerSocket serverSocket = null;
        Socket socket = null;

        try {
            // 소켓 생성
            serverSocket = new ServerSocket();
            // 포트 바인딩
            serverSocket.bind(new InetSocketAddress("192.168.20.18", 5401));

            while (true) {
                System.out.println("[연결 기다림]");
                // 연결 수락
                socket = serverSocket.accept(); // 클라이언트가 접속해 오기를 기다리고, 접속이 되면 통신용 socket 을 리턴한다.
                // 연결된 클라이언트 IP 주소 얻기
                InetSocketAddress isa = (InetSocketAddress) socket.getRemoteSocketAddress();
                System.out.println("[연결 수락함] " + isa.getHostName());

                byte[] bytes = null;
                String message = null;

                // 데이터 받기
                InputStream is = socket.getInputStream();
                bytes = new byte[100];
                int readByteCount = is.read(bytes);
                message = new String(bytes, 0, readByteCount, "UTF-8");
                System.out.println("[데이터 받기 성공] " + message);

                // 데이터 보내기
                OutputStream os = socket.getOutputStream();
                message = "Hello Client";
                bytes = message.getBytes("UTF-8");
                os.write(bytes);
                os.flush();
                System.out.println("[데이터 보내기 성공]");

                is.close();
                os.close();
                socket.close();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        if (!serverSocket.isClosed()) {
            try {
                socket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

    }

}
