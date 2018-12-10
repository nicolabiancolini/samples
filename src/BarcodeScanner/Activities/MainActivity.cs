/* 
 * BarcodeScanner (https://github.com/nicolabiancolini/Samples/tree/master/BarcodeScanner)
 * Copyright © 2017 Nicola Biancolini. All rights reserved.
 * Licensed under MIT (https://github.com/nicolabiancolini/Samples/blob/master/LICENSE)
 */

using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Widget;

using Java.IO;
using Java.Lang;

namespace BarcodeScanner.Activities
{
    [Activity(Label = "BarcodeScanner", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // Definisco la constante che sarà utilizzata per recuperare il risultato voluto nel metodo OnActivityResult.
        private const int REQUEST_IMAGE_CAPTURE = 1;

        // Definisco la chiave da impostare nello stato di istanza dell'applicazione per salvare il percorso del file temporaneo.
        private const string INSTANCE_STATE_FILE = "temporaryFile";

        // Definisco gli elementi della UI da utilizzare all'interno della Activity.
        private Button ScanButton;
        private Button ListScanButton;
        private ImageView ImageView;

        // Definisco la variabile privata che rappresenta il percorso del file temporaneo.
        private Uri fileUri;

        /// <summary>
        /// Evento scatenato alla creazione dell'activity.
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Imposta la view dell'activity.
            SetContentView(Resource.Layout.Main);

            // Recupero tutti gli oggetti dalla view mediante l'Id.
            ScanButton = FindViewById<Button>(Resource.Id.scanButton);
            ListScanButton = FindViewById<Button>(Resource.Id.listScanButton);
            ImageView = FindViewById<ImageView>(Resource.Id.imageView);

            // Assegno all'evento click del bottone ScanButton l'azione di apertura dell'app
            // utilizzata di default dal sistema per catturare l'immagine del codice da scansionare.
            ScanButton.Click += (s, e) =>
            {
                // Se il file non esiste viene creato all'interno della cache di sistema.
                if (fileUri == null) fileUri = Uri.FromFile(File.CreateTempFile("scan", ".jpg", ExternalCacheDir));

                // Creo l'intent per lanciare la camera e gli passo l'uri del file per salvare lo stream dell'immagine.
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, fileUri);

                // Lancio l'activity con StartActivityForResult per rimanere in attesa del risultato.
                StartActivityForResult(intent, REQUEST_IMAGE_CAPTURE);
            };

            // Assegno all'evento click del bottone il lancio di una nuova activity.
            ListScanButton.Click += (s, e) =>
            {
                StartActivity(new Intent(this, (typeof(ListScanActivity))));
            };
        }

        /// <summary>
        /// Evento richiamato al ritorno da una Activity lanciata con il metodo StartActivityForResult.
        /// </summary>
        /// <param name="requestCode">Identificativo della richiesta.</param>
        /// <param name="resultCode">Risultato della richiesta.</param>
        /// <param name="data">Dati passati dalla Activity.</param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Verifico la corrispondenza del codice della richiesta con quello voluto e lo stato del risultato.
            if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
            {
                // Creo il rilevatore di codice che analizzerà l'immagine.
                BarcodeDetector detector = new BarcodeDetector.Builder(this)
                    .SetBarcodeFormats(BarcodeFormat.Codabar | BarcodeFormat.Code128 | BarcodeFormat.Code39
                    | BarcodeFormat.Code93 | BarcodeFormat.DataMatrix | BarcodeFormat.Ean13 | BarcodeFormat.Ean8
                    | BarcodeFormat.Itf | BarcodeFormat.Pdf417 | BarcodeFormat.QrCode | BarcodeFormat.UpcA | BarcodeFormat.UpcE).Build();

                // Verifico che il rilevatore sia correttamente installato e operativo.
                if (!detector.IsOperational)
                {
                    Toast.MakeText(this, "Non sono ancora pronto, cerca una connessione ad internet e riprova più tardi!", ToastLength.Long).Show();
                }
                else
                {
                    try
                    {
                        // Creo l'immagine recuperandola dal file temporaneo e la imposto come contenuto della ImageView nella UI.
                        Bitmap bitmap = BitmapFactory.DecodeFile(fileUri.Path);
                        ImageView.SetImageBitmap(bitmap);

                        // Creo il frame che consente la lettura delle informazioni contenute nell'immagine.
                        Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();

                        // Recupero la lista dei codici che sono stati rilevati dal detector.
                        SparseArray barcodes = detector.Detect(frame);

                        // Se sono stati rilevati dei codici leggo solo il primo.
                        if (barcodes.Size() > 0)
                        {
                            Barcode barcode = (Barcode)barcodes.ValueAt(0);
                            Toast.MakeText(this, barcode.DisplayValue, ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(this, "Riprova, non sono stato in grado di trovare alcun codice. Forse l'immagine era sfuocata?", ToastLength.Long).Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// L'evento viene scatenato ogni volta che l'attività viene fermata ovvero prima di richiamare l'evento OnStop.
        /// </summary>
        /// <param name="outState"></param>
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            // Se il file esiste ne salvo il percorso.
            if (fileUri != null) outState.PutString(INSTANCE_STATE_FILE, fileUri.ToString());
        }

        /// <summary>
        /// L'evento viene scatenato ogni volta che l'attività viene ripresa ovvero subito prima di chiamare l'evento OnStart.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            // Se il file è stato salvato ne recupero il percorso.
            if (savedInstanceState.ContainsKey(INSTANCE_STATE_FILE)) fileUri = Uri.Parse(savedInstanceState.GetString(INSTANCE_STATE_FILE));
        }
    }
}

