//
//  MyWidgetEntryView.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

struct MyWidgetEntryView : View {
    var entry: Provider.Entry

    var body: some View {
        VStack(spacing: 4) {
            
            HStack(spacing: 8) {
                Text(entry.favoriteConfiguredEmoji)
                Text("\(entry.count)")
                    .font(.largeTitle)
                    .fontWeight(.bold)
                    .multilineTextAlignment(.center)
                    .frame(maxWidth: .infinity)
                Text(entry.favoriteConfiguredEmoji)
            }
            
            HStack(spacing: 8) {
                Button(action: {
                }) {
                    Text("-")
                        .font(.system(size: 40))
                        .frame(maxWidth: .infinity, minHeight: 44)
                        .cornerRadius(8)
                }
                
                Button(action: {
                }) {
                    Text("+")
                        .font(.system(size: 40))
                        .frame(maxWidth: .infinity, minHeight: 44)
                        .cornerRadius(8)
                }
            }
            
            Text(entry.message)
                .multilineTextAlignment(.center)
                .frame(maxWidth: .infinity)
        }
        .padding()
    }
}
