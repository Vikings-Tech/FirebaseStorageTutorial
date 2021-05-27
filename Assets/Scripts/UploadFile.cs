using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//For Picking files
using System.IO;
using SimpleFileBrowser;

//For firebase storage
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
public class UploadFile : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageReference;
    // Start is called before the first frame update
    void Start()
    {
        
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".jpg");

        
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://vikings-login.appspot.com/");


    }

    public void OnButtonClick(){
        StartCoroutine(ShowLoadDialogCoroutine());

    }

    IEnumerator ShowLoadDialogCoroutine()
    {
       
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            Debug.Log("File Selected");
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            //Editing Metadata
            var newMetadata = new MetadataChange();
            newMetadata.ContentType = "image/jpeg";

            //Create a reference to where the file needs to be uploaded
            StorageReference uploadRef = storageReference.Child("uploads/newFile.jpeg");
            Debug.Log("File upload started");
            uploadRef.PutBytesAsync(bytes, newMetadata).ContinueWithOnMainThread((task) => { 
                if(task.IsFaulted || task.IsCanceled){
                    Debug.Log(task.Exception.ToString());
                }
                else{
                    Debug.Log("File Uploaded Successfully!");
                }
            });


        }
    }
   
}
