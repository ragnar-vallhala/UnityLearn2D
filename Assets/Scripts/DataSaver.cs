using System.IO;
using System.Text;
using UnityEngine;


class DataSaver{

    private static string _filePath = "data";
    private static string _fileName = ".prefs.bin";
    private static string _tempScoreFileName = ".temp.bin";

    public static void SaveScore(int score){
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),_filePath);
        if(!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
        File.WriteAllBytes(Path.Join(fullPath,_fileName),Encrypt(Encoding.UTF8.GetBytes(score.ToString())));
    }

    
    public static int LaodScore(){
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),_filePath);
        if(!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
        if(!File.Exists(Path.Join(fullPath,_fileName)))
            return 0;
        string data = Encoding.UTF8.GetString(Decrypt(File.ReadAllBytes(Path.Join(fullPath,_fileName)))); 
        return StringToInteger(data);
    }

    public static void SaveTempScore(int score){
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),_filePath);
        if(!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
        File.WriteAllBytes(Path.Join(fullPath,_tempScoreFileName),Encrypt(Encoding.UTF8.GetBytes(score.ToString())));
    }

    public static int LaodTempScore(){
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),_filePath);
        if(!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
        if(!File.Exists(Path.Join(fullPath,_tempScoreFileName)))
            return 0;
        string data = Encoding.UTF8.GetString(Decrypt(File.ReadAllBytes(Path.Join(fullPath,_tempScoreFileName)))); 
        return StringToInteger(data);
    }

    static byte[] Encrypt(byte[] bytes){
        for(int i=0;i<bytes.Length;i++){
            bytes[i]*=2;
            if(i%2==0){
                bytes[i]+=2;
            }
            else{
                bytes[i]-=2;
            }
        }
        return bytes;
    } 
    static byte[] Decrypt(byte[] bytes){
        for(int i=0;i<bytes.Length;i++){
            if(i%2==0){
                bytes[i]-=2;
            }
            else{
                bytes[i]+=2;
            }
            bytes[i]/=2;
        }
        return bytes;
    } 

    static int StringToInteger(string data){
        int result = 0;
        for(int i=0;i<data.Length;i++){
            int num = data[i]- '0';
            result*=10;
            result+=num;
        }
        return result;
    }

}