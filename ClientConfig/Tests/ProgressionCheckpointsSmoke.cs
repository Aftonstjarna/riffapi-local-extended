using System;
using System.IO;
using System.Linq;
using ReRiff;

namespace ReRiff {internal static class NativeReelBridge {internal static void Log(string text){}}}
class ProgressionCheckpointsSmoke {
    static void Check(bool value,string text){if(!value)throw new Exception(text);}
    static void Main(){
        string root=Path.Combine(Path.GetTempPath(),"reriff-native-checkpoints-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(root);
        try {
            string path=Path.Combine(root,"reriff-sp-fixture.bin");byte[] initial={1,2,3},race={4,5,6},interrupted={9,8,7};
            File.WriteAllBytes(path,initial);ProgressionCheckpoints.Process(path,0,0);ProgressionCheckpoints.Process(path,1,1234);
            string bank=Path.Combine(root,Path.Combine("ReRiff-progression","reriff-sp-fixture"));
            var store=new ProgressionStore(bank,"singleplayer|reriff-sp-fixture");
            Check(store.Read().Progress.MainXp==1234&&store.Read().Progress.PrestigeCount==0,"Initial native load");
            File.WriteAllBytes(path,race);ProgressionCheckpoints.Process(path,2,1300);
            Check(store.Read().Progress.MainXp==1300&&store.Read().Progress.CycleXp==1300,"Native XP observation");
            Check(store.Read().NativeSave.SequenceEqual(race),"Native save snapshot");
            ProgressionCheckpoints.Process(path,0,0);ProgressionCheckpoints.Process(path,1,1300);
            Check(store.Read().Progress.Generation==2,"Restart does not reimport or advance generation");
            ProgressionCheckpoints.Process(path,2,1300);Check(store.Read().Progress.Generation==2,"Unchanged native save is not duplicated");
            File.WriteAllBytes(path,interrupted);ProgressionCheckpoints.Process(path,0,0);
            Check(File.ReadAllBytes(path).SequenceEqual(race),"Restore only committed state after interruption");
            Check(File.ReadAllBytes(path+".recovery").SequenceEqual(interrupted),"Preserve uncommitted bytes for recovery");
            File.Delete(path);ProgressionCheckpoints.Process(path,0,0);
            Check(File.ReadAllBytes(path).SequenceEqual(race),"Missing working file restores from checkpoint without cloud reimport");
            ProgressionCheckpoints.Process(path,1,1300);
            bool refused=false;try{ProgressionCheckpoints.Process(path,2,0);}catch(IOException){refused=true;}
            Check(refused&&store.Read().Progress.PrestigeCount==0,"No automatic prestige/reset from changed XP");
            Console.WriteLine("PASS: native load/save checkpoint events, permanent XP observation, restart reuse, interrupted working-copy recovery and no automatic reset");
        }finally{Directory.Delete(root,true);}
    }
}
