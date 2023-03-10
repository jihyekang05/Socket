package NaverAPI;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.Socket;

public class MySocketClient {

	// 내 ip 주소
	public static String MessageBotHost = "내 ip주소입력";

	// **수정예정
	public static int MessageBotPort = 1629;


	public static String MessageBotDelimeter = "#kisNaverBotSplit#";

	// MessageBot.cs의 SendMessage함수
	public static String SendMessage(String userList, String actCode, String sendTitle, String message,
			String resultMsg, String Info) {

		String cmd = null;
		cmd = userList + MessageBotDelimeter + sendTitle + MessageBotDelimeter + message + MessageBotDelimeter
				+ resultMsg + MessageBotDelimeter + actCode + MessageBotDelimeter + Info;

		try {
			Socket socket = new Socket(MessageBotHost, MessageBotPort);
			System.out.println("소켓 서버 접속완료");

			// Client -> Server로 전송
			PrintWriter pr = new PrintWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8"), true);

			// 클라이언트가 보낸 메시지 출력
			pr.println(cmd);

			// 서버가 보낸 메시지 읽기
			BufferedReader br = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));

			// 서버로부터 온 메세지 확인
			System.out.println(br.readLine());

			// 소켓 종료
			System.out.println("CLIENT SOCKET CLOSE");
			socket.close();

			return "OK : " + cmd;

		} catch (Exception e) {

			e.printStackTrace();
			System.out.println("NaverBot Server Error");
			
			return "ERROR : NaverBot Server Error";
		}

	}

	public static void main(String[] args) {

		SendMessage("it_data@kispricing.com;", "3", "[TEST]", "로이터 IRS 금리 (16:00)", "확인", "인포");

	}

}
