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
		try {
			int socketPort = 1629;
			ServerSocket socket = new ServerSocket(socketPort);
			Socket socketUser = null;
			System.out.println("socket : " + socketPort + "으로 서버 열림");

			while (true) {
				socketUser = socket.accept();
				System.out.println("client 접속 : " + socketUser.getInetAddress());

				// 클라이언트 -> 서버
				InputStream input = socketUser.getInputStream();
				BufferedReader br = new BufferedReader(new InputStreamReader(input));

				// 클라이언트가 보낸 메시지 출력
				System.out.println(br.readLine());

				// 서버 -> 클라이언트로 메시지 보내는 부분
				OutputStream out = socketUser.getOutputStream();
				PrintWriter pr = new PrintWriter(out, true);

				pr.println("SERVER TO CLIENT");
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
